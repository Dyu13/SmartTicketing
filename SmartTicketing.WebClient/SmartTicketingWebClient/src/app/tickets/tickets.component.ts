import { Component, OnInit } from '@angular/core';
import { TicketModel } from '../models/ticket.model';
import { TicketService } from '../services/ticket.service';

@Component({
  selector: 'app-tickets',
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.scss']
})
export class TicketsComponent implements OnInit {

  public PageList: number[] = []; // TODO: better use pipes
  public TicketList: TicketModel[] = []
  public SelectedPage: number = 1;

  constructor(private ticketService: TicketService) { }

  ngOnInit(): void {
    this.ticketService.getCount().subscribe((count: number) => {
      var pageCount = count / 100;
      for (let i = 1; i <= pageCount; i++) {
        this.PageList.push(i);
      }
    });

    this.getData();
  }

  lowerPage(): void {
    if (this.SelectedPage < 2) return;
    this.SelectedPage--;
    this.getData();
  }

  higherPage(): void {
    if (this.SelectedPage >= this.PageList.length) return;
    this.SelectedPage++;
    this.getData();
  }

  pageClick(page: number) {
    this.SelectedPage = page;
    this.getData();
  }

  getData(): void {
    this.ticketService.get(this.SelectedPage).subscribe((data: any) => {
      this.TicketList = data.value;
    })
  }

  getSummary(ticket: TicketModel) {
    this.ticketService.getSummaryByDescription(ticket.Description);
  }

}
