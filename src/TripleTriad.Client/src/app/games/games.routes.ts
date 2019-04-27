import { Routes } from '@angular/router';
import { RoomComponent } from './containers/room/room.component';

export const routes: Routes = [{ path: ':gameId', component: RoomComponent }];
