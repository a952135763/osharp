// -----------------------------------------------------------------------
//  <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//  </once-generated>
//
//  <copyright file="channel.module.ts">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@shared';
import { ChannelRoutingModule } from './channel.routing';
import { ChannelsComponent } from './channels/channels.component';
import { PercentageComponent } from './percentage/percentage.component';
import { UserChannelComponent } from './user-channel/user-channel.component';
import { ChannelTypeComponent } from './channel-type/channel-type.component';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    ChannelRoutingModule
  ],
  declarations: [
    ChannelsComponent,
    PercentageComponent,
    UserChannelComponent,
    ChannelTypeComponent,
  ]
})
export class ChannelModule { }
