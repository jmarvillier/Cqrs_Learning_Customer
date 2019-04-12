import { Component, OnInit } from '@angular/core';
import {CUSTOMERS} from '../mock-customers';
import { Customer } from '../customer';
import { CustomerService } from '../customer.service';
import { MessageService } from '../message.service';

@Component({
  selector: 'app-customers',
  templateUrl: './customers.component.html',
  styleUrls: ['../style.css']
})

export class CustomersComponent implements OnInit {
  constructor(private customerService: CustomerService,
    public messageService: MessageService) { }

  ngOnInit() {
    this.getCustomers();
  }

  customers: Customer[];
  selectedCustomer: Customer;

  onSelect(customer: Customer):void{
    this.messageService.add(`Customer (${customer.Id}#) ${customer.Name} selected`);
    this.selectedCustomer = customer;
  }

  getCustomers():void{
    this.customerService.getCustomers().subscribe(customers=> this.customers = customers);
  }
}