﻿import { Injectable, Inject, NgZone } from '@angular/core';

@Injectable()
export class GlobalUrlService {
    // urls to server for all components
    public static getAllOrganizationsUrl: string = "api/OrganizationDetail";
    public static getOrganizationDetailUrl: string = "api/OrganizationDetail/OrganinzationDetailByOrgId/";

    //organization account
    public static getExtractStatus: string = "api/OrgAccount/ExtractStatus/";
    public static getExtractCredentials: string = "api/OrgAccount/ExtractCredentials/";
    public static connectExtracts: string = "api/OrgAccount/ConnectExtracts";

    //donation URLs
    public static userDonations: string = "api/Donate/User/";
    public static userDonationsByDate: string = "api/Donate/UserByDate/";
}