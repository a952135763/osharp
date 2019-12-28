// -----------------------------------------------------------------------
//  <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//  </once-generated>
//
//  <copyright file="provide.routing.ts">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ACLGuard } from '@delon/acl';
import { ArticleAssortComponent } from './article-assort/article-assort.component';
import { ArticleEntitiesComponent } from './article-entities/article-entities.component';
import { PointsComponent } from './points/points.component';
import { PointsLogComponent } from './points-log/points-log.component';
import { ProvideExtraComponent } from './provide-extra/provide-extra.component';
import { SubscriptionComponent } from './subscription/subscription.component'

const routes: Routes = [
  { path: 'article-assort', component: ArticleAssortComponent, canActivate: [ACLGuard], data: { title: '收款分类列表管理', reuse: true, guard: 'Root.Admin.Provide.ArticleAssort.Read' } },
  { path: 'article-entities', component: ArticleEntitiesComponent, canActivate: [ACLGuard], data: { title: '收款号列表管理', reuse: true, guard: 'Root.Admin.Provide.ArticleEntities.Read' } },
  { path: 'points', component: PointsComponent, canActivate: [ACLGuard], data: { title: '供应商实时积分管理', reuse: true, guard: 'Root.Admin.Provide.Points.Read' } },
  { path: 'points-log', component: PointsLogComponent, canActivate: [ACLGuard], data: { title: '积分变动记录管理', reuse: true, guard: 'Root.Admin.Provide.PointsLog.Read' } },
  { path: 'provide-extra', component: ProvideExtraComponent, canActivate: [ACLGuard], data: { title: '供应商参数管理', reuse: true, guard: 'Root.Admin.Provide.ProvideExtra.Read' } },
  { path: 'subscription' ,component: SubscriptionComponent,data: { title: '个码抢单', reuse: true } }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProvideRoutingModule { }
