import { Injectable } from '@angular/core';
import { Customer } from './customer';
import { CUSTOMERS } from './mock-customers';
import { Observable, of } from 'rxjs';
import { MessageService } from './message.service';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {

  constructor(private messageService: MessageService) { }

  getCustomers(): Observable<Customer[]>{
    this.messageService.add('CustomerService: fetched customers.')
    return of(CUSTOMERS);
  }
}
