
export interface IEvent {
    Time: string;
    ResourceId: string;
    OperationName: string;
    Category: string;
    ResultType: string;
    ResultSignature: string;
    DurationMs: string;
    CallerIpAddress: string;
    CorrelationId: string;
    Level: string;
    Location: string;
}
