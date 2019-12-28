// -----------------------------------------------------------------------
//  <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//  </once-generated>
//
//  <copyright file="cash-money.routing.ts">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ACLGuard } from '@delon/acl';
import { BankListComponent } from './bank-list/bank-list.component';
import { CashLogComponent } from './cash-log/cash-log.component';

const routes: Routes = [
  { path: 'bank-list', component: BankListComponent, canActivate: [ACLGuard], data: { title: '收款账号管理', reuse: true, guard: 'Root.Admin.CashMoney.BankList.Read' } },
  { path: 'cash-log', component: CashLogComponent, canActivate: [ACLGuard], data: { title: '提现记录管理', reuse: true, guard: 'Root.Admin.CashMoney.CashLog.Read' } },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CashMoneyRoutingModule { }
