
export class LogPage {
    data: Log[];
    nextPageStartId: string;
}

export class Log {
    logId: string;
    applicationId: string;
    timeStamp: Date;
    level: string;
    message: string;
    exception: Exception;
}

export class Exception {
    stackTrace: string;
    source: string;
    message: string;
    innerException: Exception;
}
