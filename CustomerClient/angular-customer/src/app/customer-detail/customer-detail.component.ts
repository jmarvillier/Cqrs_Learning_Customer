import { Component, OnInit, Input } from '@angular/core';
import {Customer} from '../customer'

@Component({
  selector: 'app-customer-detail',
  templateUrl: './customer-detail.component.html',
  styleUrls: ['../style.css']
})
export class CustomerDetailComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  @Input() customer:Customer;
}
