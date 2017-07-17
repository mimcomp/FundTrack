﻿import { Component, OnInit } from "@angular/core";
import { IEventManagementViewModel } from "../../view-models/abstract/organization-management-view-models/event-management-view-model.interface";
import { OrganizationManagementEventsService } from "../../services/concrete/organization-management/organization-management-events.service";
import { Subscription } from "rxjs/Subscription";
import { ActivatedRoute, Router } from "@angular/router";

@Component({
    selector: 'org-management-event',
    templateUrl: './organization-manadement-event-edit.component.html',
    styleUrls: ['./organization-manadement-event-edit.component.css']
})
export class OrganizationManadementEventEditComponent implements OnInit {
    private _idForCurrentEvent: number;
    private _event: IEventManagementViewModel;
    private _subscription: Subscription;

    /**
     * @constructor
     * @param _route: ActivatedRoute
     * @param _router: Router
     * @param _service: OrganizationManagementEventsService
     */
    public constructor(private _route: ActivatedRoute, private _router: Router, private _service: OrganizationManagementEventsService) { }

    ngOnInit(): void {
        this._subscription = this._route.params.subscribe(params => {
            this._idForCurrentEvent = +params['id'];
            this.getInformationOfEvent(this._idForCurrentEvent);
        });
    }

    /**
     * Gets one event by identifier
     * @param id
     */
    public getInformationOfEvent(id: number): void{
        this._service.getOneEventById(id).subscribe(event => this._event = event);
    }

    /**
     * Updates event
     */
    private updateEvent(): void{
        this._service.updateEvent(this._event)
            .subscribe(ev => {
                console.log(ev)
            });
        this._router.navigate(['organization/events/' + this._event.organizationId]);
    }

    ngDestroy(): void {
        this._subscription.unsubscribe();
    }
}