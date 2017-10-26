﻿import { ViewChild, Component, OnInit, Input, SimpleChange, OnChanges, Output, EventEmitter } from "@angular/core";
import { Router } from "@angular/router";
import { OrgAccountService } from "../../services/concrete/finance/orgaccount.service";
import { OrgAccountViewModel } from "../../view-models/concrete/finance/orgaccount-viewmodel";
import { FixingBalanceService } from "../../services/concrete/fixing-balance.service";
import { DecimalPipe } from '@angular/common';
import { CurrencyPipe } from '@angular/common';
import * as key from '../../shared/key.storage';
import * as constant from '../../shared/default-configuration.storage';
import * as message from '../../shared/common-message.storage';
import { FinOpService } from "../../services/concrete/finance/finOp.service";
import { FinOpListViewModel } from "../../view-models/concrete/finance/finop-list-viewmodel";
import { isBrowser } from "angular2-universal";
import { ModalComponent } from "../../shared/components/modal/modal-component";
import { MoneyOperationViewModel } from "../../view-models/concrete/finance/money-operation-view-model";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { ValidatorsService } from "../../services/concrete/validators/validator.service";
import Core = require("@angular/core");
import Targetviewmodel = require("../../view-models/concrete/finance/donate/target.view-model");
import { Image } from "../../view-models/concrete/image.model";
import { EditOrganizationService } from "../../services/concrete/organization-management/edit-organization.service";
import { ModeratorViewModel } from '../../view-models/concrete/edit-organization/moderator-view.model';
import { AuthorizeUserModel } from "../../view-models/concrete/authorized-user-info-view.model";
import { DonateService } from "../../services/concrete/finance/donate-money.service";
import { CurrencyViewModel } from "../../view-models/concrete/finance/donate/currency.view-model";
import { TargetViewModel } from "../../view-models/concrete/finance/donate/target.view-model";
import { DeleteOrgAccountViewModel } from "../../view-models/concrete/finance/deleteorgaccount-view.model";
import { OrgAccountDetailComponent } from "../../components/finance/orgaccountdetail.component";
import { DonateViewModel } from "../../view-models/concrete/finance/donate/donate.view-model";
import { UserInfo } from "../../view-models/concrete/user-info.model";
import { Location } from '@angular/common';
import { ActivatedRoute } from "@angular/router";
import { UserService } from "../../services/concrete/user.service";

@Component({
    selector: 'incomeoperation',
    templateUrl: './incomeoperation.component.html',
    styleUrls: ['./incomeoperation.component.css'],
    providers: [DonateService, OrgAccountService, EditOrganizationService, UserService, FixingBalanceService]
})

export class IncomeOperationComponent {

    private moneyIncomeForm: FormGroup;
    private moneyOperationModel: FinOpListViewModel = new FinOpListViewModel();
    private moneyIncome: FinOpListViewModel = new FinOpListViewModel();
    private cashAccounts: OrgAccountViewModel[] = new Array<OrgAccountViewModel>();
    private currentAccount: OrgAccountViewModel = new OrgAccountViewModel();
    private currentDate = new Date().toJSON().slice(0, 10);
    private currentAccountId: number;
    private currentTarget: TargetViewModel = new TargetViewModel();
    private user: AuthorizeUserModel = new AuthorizeUserModel();
    private minDate: string;
    private isAccountChosen: boolean = false;
    private isAccountKnown: boolean = false;

    public constructor(private router: Router,
        private location: Location,
        private route: ActivatedRoute,
        private finOpService: FinOpService,
        private fb: FormBuilder,
        private validatorsService: ValidatorsService,
        private accountService: OrgAccountService,
        private donateService: DonateService,
        private userService: UserService,
        private editService: EditOrganizationService,
        private fixingService: FixingBalanceService) {
        this.createIncomeForm();
    }

    ngOnInit(): void {
        this.initialize();
    }

    private getCurrentOrgAccount(accountId: number) {
        this.accountService.getOrganizationAccountById(accountId)
            .subscribe(currAcc => {
                this.currentAccount = currAcc;
                this.getCurrentTarget();
                this.getMinDate();
            });
    }
    
    private initialize() {
        this.moneyOperationModel.date = new Date();
        this.getLoggedUser();
        this.route.params.subscribe(params => {
            this.currentAccountId = params['id'];
            if (this.currentAccountId == undefined) {
                this.getOrganizationCashAccounts();
            }
            else {
                this.isAccountKnown = true;
                this.isAccountChosen = true;
                this.getCurrentOrgAccount(this.currentAccountId);
            }
        });
    }

    private onAccountSelect(accountId: number) {
        this.getCurrentOrgAccount(accountId);
        this.isAccountChosen = true;
    }

    private getCurrentTarget() {
        if (this.currentAccount.targetId == null) {
            this.currentTarget.name = message.nullTarget;
        }
        else {
            this.accountService.getTargetById(this.currentAccount.targetId).subscribe(target => {
                this.currentTarget = target;
            });
        }
    }

    public getMinDate() {
        this.fixingService.getFilterByAccId(this.currentAccount.id)
            .subscribe(fix => {
                if (fix.lastFixing.balanceDate == null) {
                    this.minDate = this.currentAccount.creationDate.toJSON().slice(0, 10);
                }
                else {
                    this.minDate = fix.lastFixing.balanceDate.slice(0, 10);
                }
            });
    }

    private getOrganizationCashAccounts() {
        this.accountService.getAllAccountsOfOrganization()
            .subscribe(acc => {
                this.cashAccounts = acc.filter(a =>
                    a.accountType == constant.cashUA
                );
            });
    }

    public getLoggedUser() {
        this.user = JSON.parse(localStorage.getItem(key.keyModel)) as AuthorizeUserModel;
    }

    public setDate(model: FinOpListViewModel, date: Date) {
        model.date = date;
    }

    private createIncomeForm() {
        this.moneyIncomeForm = this.fb.group({
            cardToId: [
                this.moneyOperationModel.cardToId
            ],
            amount: [
                this.moneyOperationModel.amount, [Validators.required,
                this.validatorsService.isMinValue,
                this.validatorsService.isMaxValue,
                this.validatorsService.isNumber
                ]
            ],
            targetId: [
                this.moneyOperationModel.targetId
            ],
            description: [
                this.moneyOperationModel.description, [Validators.maxLength(500)]
            ],
            date: [
                this.moneyOperationModel.date
            ]
        });
        this.moneyIncomeForm.valueChanges
            .subscribe(a => this.onValueChange(a));
        this.onValueChange();
    }

    private formIncomeErrors = {
        amount: "",
        description: ""
    };

    private invalidAmountMessage = message.invalidAmountMessage;
    private maxLengthDescription = message.maxLengthDescription;

    private validationMessages = {
        amount: {
            notnumber: this.invalidAmountMessage,
            notminvalue: this.invalidAmountMessage,
            notmaxvalue: this.invalidAmountMessage
        },
        description: {
            maxlength: this.maxLengthDescription
        }
    }

    private onValueChange(data?: any) {
        if (!this.moneyIncomeForm) return;
        let form = this.moneyIncomeForm;

        for (let field in this.formIncomeErrors) {
            this.formIncomeErrors[field] = "";
            let control = form.get(field);

            if (control && control.dirty && !control.valid) {
                let message = this.validationMessages[field];
                for (let key in control.errors) {
                    this.formIncomeErrors[field] = message[key.toLowerCase()];
                }
            }
        }
    }

    private makeIncome() {
        this.completeModel();
        console.log(this.moneyOperationModel);
        this.finOpService.createIncome(this.moneyOperationModel).subscribe(a => {
            if (a.error == "" || a.error == null) {
                this.showToast();
                setTimeout(() => {
                    this.router.navigate(['finance/orgaccounts']);
                },
                    2000);

            } else {
                this.moneyOperationModel.error = a.error;
            }
        });
    }

    private completeModel() {
        this.moneyOperationModel.targetId = this.currentTarget.targetId;
        this.moneyOperationModel.cardToId = this.currentAccount.id;
        this.moneyOperationModel.finOpType = constant.incomeId;
        this.moneyOperationModel.orgId = this.user.orgId;
        this.moneyOperationModel.userId = this.user.id;
    }

    public showToast() {
        var x = document.getElementById("snackbar");
        x.className = "show";
        setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);
    }


    private navigateBack(): void {
        this.location.back();
    }
}