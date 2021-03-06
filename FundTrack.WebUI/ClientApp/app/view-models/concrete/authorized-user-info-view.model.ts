﻿import { UserInfo } from './user-info.model';
/**
 * model which used when return authorize user
 */
import { ValidationViewModel } from "./validation-view.model";

export class AuthorizeUserModel {
    public login: string;
    public id: number;
    public firstName: string;
    public lastName: string;
    public email: string;
    public address: string;
    public photoUrl: string;
    public role: string;
    public orgId: number;
    public phone: string;
}

export class AuthorizedUserInfoViewModel {
    public userModel: AuthorizeUserModel;
    public access_token: string;
    public errorMessage: string;
    public validationSummary: ValidationViewModel[];
}
