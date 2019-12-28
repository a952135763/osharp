// -----------------------------------------------------------------------
//  <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//  </once-generated>
//
//  <copyright file="provide.module.ts">
//      KaPai©2019 Microsoft Corporation. All rights reserved.
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@shared';
import { ProvideRoutingModule } from './provide.routing';
import { ArticleAssortComponent } from './article-assort/article-assort.component';
import { ArticleEntitiesComponent } from './article-entities/article-entities.component';
import { PointsComponent } from './points/points.component';
import { PointsLogComponent } from './points-log/points-log.component';
import { ProvideExtraComponent } from './provide-extra/provide-extra.component';
import { SubscriptionComponent } from './subscription/subscription.component';

import { ClipboardModule } from 'ngx-clipboard';
import { SortableModule } from '@progress/kendo-angular-sortable';

@NgModule({
  imports: [CommonModule, SharedModule, ProvideRoutingModule, SortableModule,ClipboardModule],
  declarations: [
    ArticleAssortComponent,
    ArticleEntitiesComponent,
    PointsComponent,
    PointsLogComponent,
    ProvideExtraComponent,
    SubscriptionComponent,
  ],
})
export class ProvideModule {}
