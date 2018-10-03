import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { LogsHubService } from '../../services/logs-hub/logs-hub.service';
import { LogPage, Log, Exception } from '../../data/log';
import { animate, state, style, transition, trigger} from '@angular/animations';

@Component({
  selector: 'alui-logs-dashboard',
  templateUrl: './logs-dashboard.component.html',
  styleUrls: ['./logs-dashboard.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0', display: 'none'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})
export class LogsDashboardComponent implements OnInit, OnDestroy {

  columnsToDisplay: string[] = ['applicationId', 'timeStamp', 'level', 'shortMessage'];
  expandedElement: LogDetails;
  logs: LogRow[] = [];
  isWaitingResultsPage = true;

  constructor(private logsService: LogsHubService) { }

  ngOnInit() {
    this.logsService.connect();
    this.logsService.logReceived().subscribe(log => {
      this.appendLog(log);
    });
    this.logsService.logPageReceived().subscribe(page => {
      for (const log of (<LogPage>page).data) {
        this.appendLog(log);
      }
      this.isWaitingResultsPage = false;
    });
  }

  onScroll() {
    if (this.isWaitingResultsPage) {
      return;
    }
    this.isWaitingResultsPage = true;
    this.logsService.loadNextLogsPage();
  }

  ngOnDestroy(): void {
    this.logsService.disconnect();
  }

  reconnect() {
    this.logsService.connect();
  }

  isDisconnected() {
    return this.logsService.isDisconnected();
  }

  isWarning(log: LogRow) {
    return log.level.toLowerCase() === 'warning';
  }

  isError(log: LogRow) {
    return log.level.toLowerCase() === 'error' || log.level.toLowerCase() === 'fatal';
  }

  getExceptionDetails(exception: Exception) {
    if (!exception) {
      return '';
    }

    let details = 'Message: ' + exception.message + '<br/> ' +
      'Source: ' + exception.source +
      'StackTrace:<br/>' + exception.stackTrace.replace('\n', '<br/>').replace('\r', '<br/>');
    if (exception.innerException) {
      details += '<br/>Inner exception:<br/>' + this.getExceptionDetails(exception.innerException);
    }
    return details;
  }

  private appendLog(log: Log) {
    const convertedLog = this.convertToLogRow(log);
    this.logs.push(convertedLog);
  }

  private convertToLogRow(log: Log) {
    return <LogRow>{
      applicationId: log.applicationId,
      timeStamp: log.timeStamp,
      level: log.level,
      shortMessage: log.message.length > 100 ? log.message.substr(0, 100) + '...' : log.message,
      details: <LogDetails>{
        fullMessage: log.message,
        exception: log.exception
      }
    };
  }
}

export class LogRow {
  applicationId: string;
  timeStamp: Date;
  level: string;
  shortMessage: string;
  details: LogDetails;
}

export class LogDetails {
  fullMessage: string;
  exception: Exception;
}
