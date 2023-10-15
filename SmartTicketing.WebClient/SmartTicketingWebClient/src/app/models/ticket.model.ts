export interface TicketModel {
    TicketId: number;
    Title: string;
    Description: string;
    Status: number;
    CreatedByUserId: number;
    AssignToUserId: number;
}