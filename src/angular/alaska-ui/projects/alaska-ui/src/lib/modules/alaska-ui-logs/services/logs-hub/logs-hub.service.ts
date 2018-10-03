import { Injectable, EventEmitter } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { Log, LogPage } from '../../data/log';
import { NotificationService } from '../../../alaska-ui-core/services/notification.service';
import { InjectionToken } from '@angular/core';
import { Optional } from '@angular/core';
import { Inject } from '@angular/core';

export const DEFAULT_LOGS_HUB_URL = new InjectionToken<string>('DEFAULT_LOGS_HUB_URL');

@Injectable({
  providedIn: 'root'
})
export class LogsHubService {

  private disconnected = true;
  private logsHubUrl = '/hub/logs';
  private connection: HubConnection;
  private logReceivedEvent = new EventEmitter<Log>();
  private logPageReceivedEvent = new EventEmitter<LogPage>();
  private pageSize = 50;
  private nextLogsPageId = '';
  private isWaitingNextPage = false;

  constructor(
    private notificationService: NotificationService,
    @Optional() @Inject(DEFAULT_LOGS_HUB_URL) logsHubUrl?: string) {
      if (logsHubUrl) {
        this.logsHubUrl = logsHubUrl;
      }
    }

  connect() {
    this.establishConnection(this.logsHubUrl);
  }

  logReceived() {
    return this.logReceivedEvent;
  }

  logPageReceived() {
    return this.logPageReceivedEvent;
  }

  loadNextLogsPage() {
    if (!this.nextLogsPageId || this.isWaitingNextPage) {
      return;
    }
    this.isWaitingNextPage = true;
    this.connection.invoke('GetLogsPage', this.nextLogsPageId, this.pageSize);
  }

  disconnect() {
    this.closeConnection();
  }

  changeTarget(url: string) {
    this.closeConnection();
    this.logsHubUrl = url;
    this.establishConnection(url);
  }

  isDisconnected() {
    return this.disconnected;
  }

  private establishConnection(url) {
    this.openConnection(url);
  }

  private registerHandlers() {

    this.connection.invoke('GetFirstLogsPage', this.pageSize)
      .catch(error => {
        this.notificationService.showErrorDialog({
          title: 'Initialization Error',
          message: 'Logs initialization error',
          errorDetails: error.message
        });
        this.closeConnection();
      });

    this.connection.on('receiveFirstLogsPage', (logs: LogPage) => {
      console.log('first logs page received');
      this.nextLogsPageId = logs.nextPageStartId;
      this.isWaitingNextPage = false;
      this.logPageReceivedEvent.emit(logs);
    });

    this.connection.on('receiveLogsPage', (logs: LogPage) => {
      console.log('logs page received');
      this.nextLogsPageId = logs.nextPageStartId;
      this.logPageReceivedEvent.emit(logs);
    });

    this.connection.on('receiveNewLog', (log: Log) => {
      console.log('log received');
      this.logReceivedEvent.emit(log);
    });

    this.connection.onclose(error => {
      this.disconnected = true;
      if (error) {
        this.notificationService.showErrorDialog({
          title: 'Connection Error',
          message: 'Disconnected from hub',
          errorDetails: error.message
        });
      }
    });
  }

  private openConnection(url) {

    this.disconnected = false;
    this.connection = new HubConnectionBuilder()
      .withUrl(url)
      .build();

    this.connection.start()
      .then(() => {
        this.registerHandlers();
      })
      .catch(err => {
        this.disconnected = true;
        this.notificationService.showErrorDialog({
          title: 'Connectin Error',
          message: 'Error connecting to hub ' + url,
          errorDetails: err
        });
      });
  }

  private closeConnection() {
    this.connection.stop();
  }
}
