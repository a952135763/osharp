// -----------------------------------------------------------------------
//  <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//  </once-generated>
//
//  <copyright file="merchant.routing.ts">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ACLGuard } from '@delon/acl';
import { OrdersComponent } from './orders/orders.component';
import { AmountsComponent } from './amounts/amounts.component';
import { AmountsLogComponent } from './amounts-log/amounts-log.component';
import { MerchantExtraComponent } from './merchant-extra/merchant-extra.component';

const routes: Routes = [
  { path: 'orders', component: OrdersComponent, canActivate: [ACLGuard], data: { title: '商户订单列表管理', reuse: true, guard: 'Root.Admin.Merchant.Orders.Read' } },
  { path: 'amounts', component: AmountsComponent, canActivate: [ACLGuard], data: { title: '商户实时余额管理', reuse: true, guard: 'Root.Admin.Merchant.Amounts.Read' } },
  { path: 'amounts-log', component: AmountsLogComponent, canActivate: [ACLGuard], data: { title: '余额变动记录管理', reuse: true, guard: 'Root.Admin.Merchant.AmountsLog.Read' } },
  { path: 'merchant-extra', component: MerchantExtraComponent, canActivate: [ACLGuard], data: { title: '商户参数管理', reuse: true, guard: 'Root.Admin.Merchant.MerchantExtra.Read' } },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MerchantRoutingModule { }
