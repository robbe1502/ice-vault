import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';

// Modules
import { SharedModule } from '../shared/shared.module';

// Containers
import { LoginComponent } from './containers/login/login.component';
import { LayoutComponent } from './containers/layout/layout.component';

// Components
import { LoginFormComponent } from './components/login-form/login-form.component';

// Routes
const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      {
        path: 'login',
        component: LoginComponent
      },
      {
        path: '**',
        redirectTo: 'login'
      }
    ]
  },
  {
    path: '**',
    redirectTo: ''
  }
];

@NgModule({
  imports: [CommonModule, RouterModule.forChild(routes), ReactiveFormsModule, TranslateModule, SharedModule],
  declarations: [LoginComponent, LayoutComponent, LoginFormComponent]
})
export class AuthenticationModule {}
