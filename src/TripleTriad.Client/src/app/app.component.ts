import { Component, OnInit } from '@angular/core';
import { SignalRFacade } from './core/services/signal-r.facade';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  constructor(private signalRFacade: SignalRFacade) {}

  ngOnInit(): void {
    //this.signalRFacade.connect();
  }

  title = 'app';
}
