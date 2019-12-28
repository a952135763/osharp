// -----------------------------------------------------------------------
//  <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//  </once-generated>
//
//  <copyright file="merchant.module.ts">
//      KaPai©2019 Microsoft Corporation. All rights reserved.
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@shared';
import { MerchantRoutingModule } from './merchant.routing';
import { OrdersComponent } from './orders/orders.component';
import { AmountsComponent } from './amounts/amounts.component';
import { AmountsLogComponent } from './amounts-log/amounts-log.component';
import { MerchantExtraComponent } from './merchant-extra/merchant-extra.component';
import { OrderBackLogComponent } from './order-back-log/order-back-log.component';

@NgModule({
  imports: [CommonModule, SharedModule, MerchantRoutingModule],
  declarations: [OrdersComponent, AmountsComponent, AmountsLogComponent, MerchantExtraComponent, OrderBackLogComponent],
})
export class MerchantModule {}
