// -----------------------------------------------------------------------
//  <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//  </once-generated>
//
//  <copyright file="cash-money.module.ts">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@shared';
import { CashMoneyRoutingModule } from './cash-money.routing';
import { BankListComponent } from './bank-list/bank-list.component';
import { CashLogComponent } from './cash-log/cash-log.component';


@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    CashMoneyRoutingModule
  ],
  declarations: [
    BankListComponent,
    CashLogComponent,
  ]
})
export class CashMoneyModule { }
