// -----------------------------------------------------------------------
//  <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//  </once-generated>
//
//  <copyright file="channel.routing.ts">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ACLGuard } from '@delon/acl';
import { ChannelsComponent } from './channels/channels.component';
import { PercentageComponent } from './percentage/percentage.component';
import { UserChannelComponent } from './user-channel/user-channel.component';
import { ChannelTypeComponent } from './channel-type/channel-type.component';

const routes: Routes = [
  { path: 'channels', component: ChannelsComponent, canActivate: [ACLGuard], data: { title: '通道列表管理', reuse: true, guard: 'Root.Admin.Channel.Channels.Read' } },
  { path: 'percentage', component: PercentageComponent, canActivate: [ACLGuard], data: { title: '费率列表管理', reuse: true, guard: 'Root.Admin.Channel.Percentage.Read' } },
  { path: 'user-channel', component: UserChannelComponent, canActivate: [ACLGuard], data: { title: '用户开启通道管理', reuse: true, guard: 'Root.Admin.Channel.UserChannel.Read' } },
  { path: 'channel-type', component: ChannelTypeComponent, canActivate: [ACLGuard], data: { title: '通道供应商账号类型管理', reuse: true, guard: 'Root.Admin.Channel.ChannelType.Read' } },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ChannelRoutingModule { }
