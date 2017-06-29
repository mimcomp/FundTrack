﻿import { Component, Injectable, Input, ElementRef, ViewChild, NgZone, OnInit } from '@angular/core';
import { IMarker } from "../../../models/map/marker.interface";
import { FormControl } from "@angular/forms";
import { MapsAPILoader, LatLngLiteral } from "@agm/core";
import { } from '@types/googlemaps';
import { Observable } from "rxjs/Observable";

//https://angular-maps.com/api-docs/agm-core/components/AgmInfoWindow.html documentation
//https://developers.google.com/maps/documentation/geocoding/intro

@Component({
    selector: 'map-component',
    templateUrl: './map.component.html',
    styleUrls: ['./map.component.css']
})

@Injectable()
export class MapComponent implements OnInit {
    private _maximumConcurentRequestsToGoogleMap: number;
    private _temporaryAddressForAutocomplete: string;
    private _addresses: string[] = [];
    private _markers: IMarker[] = [];

    //Latitude can be initialize through attributes in html
    @Input()
    public mainPointerLatitude: number;

    //Longitude can be initialize through attributes in html
    @Input()
    public mainPointerLongitude: number;

    //The scale of the map
    @Input()
    public zoom: number;

    //Attribute for permission to use map in many markers mode
    @Input()
    public allowManyMarkers: boolean;

    //For autocomplete form
    @Input()
    public searchControl: FormControl;

    //For autocomplete form
    @ViewChild("search")
    public searchElementRef: ElementRef;

    /**
     * @constructor
     * @param _mapsAPILoader
     * @param _ngZone
     */
    constructor(private _mapsAPILoader: MapsAPILoader, private _ngZone: NgZone) { }

    ngOnInit(): void {
        this._maximumConcurentRequestsToGoogleMap = 5;
        //create search FormControl
        this.searchControl = new FormControl();

        //load Places Autocomplete
        this._mapsAPILoader.load().then(() => {
            let autocomplete = new google.maps.places.Autocomplete(this.searchElementRef.nativeElement,
                {
                    types: ["address"]
                });
            autocomplete.addListener("place_changed", () => {
                this._ngZone.run(() => {
                    //get the place result
                    let place: google.maps.places.PlaceResult = autocomplete.getPlace();
                    let geo = google.maps.Geocoder;
                    //verify result
                    if (place.geometry === undefined || place.geometry === null) {
                        return;
                    }
                    //set latitude, longitude and zoom
                    this.setMainPointer(place.geometry.location.lat(), place.geometry.location.lng(), 12);
                    this._temporaryAddressForAutocomplete = place.formatted_address;
                });
            });
        });
    }

    /**
     * Here is confirm window. Asks for use current location
     */
    ngAfterViewInit() {
        if (confirm("Дозволити використовувати Ваше поточне місцезнаходження")) {
            this.setMainPointerOnCurrentLocation();
        }
        else {
            //set main pointer on the Lviv City Hall
            this.setMainPointer(49.841936, 24.031591, 9);
        }
    }

    /**
     * Sets current location on map
     */
    private setMainPointerOnCurrentLocation() {
        if ("geolocation" in navigator) {
            navigator.geolocation.getCurrentPosition((position) => {
                this.setMainPointer(position.coords.latitude, position.coords.longitude, 9);
            });
        }
    }

    /**
     * Sets main pointed on coordinates
     * @param latitude
     * @param longitude
     */
    private setMainPointer(latitude: number, longitude: number, zoom?: number): void {
        this.mainPointerLatitude = latitude;
        this.mainPointerLongitude = longitude;
        if (zoom != null) {
            this.zoom = zoom;
        }
    }

    /**
     * Saves address in _addresses from autocomplete form, saves new marker if allowed many markers
     * Updates address if no allowed many markers
     */
    private saveMarkerAndAddressFromDataInAutocompleteForm(): void {
        if (this._temporaryAddressForAutocomplete) {
            if (this.allowManyMarkers) {
                var addressContainsInArray = this._addresses.find(a => a == this._temporaryAddressForAutocomplete);
                if (!addressContainsInArray) {
                    this._addresses.push(this._temporaryAddressForAutocomplete);
                    this._markers.push(this.createNewMarker(this.mainPointerLatitude, this.mainPointerLongitude, this._temporaryAddressForAutocomplete));
                }
            } else {
                this._addresses[0] = this._temporaryAddressForAutocomplete;
                this._markers[0] = this.createNewMarker(this.mainPointerLatitude, this.mainPointerLongitude, this._temporaryAddressForAutocomplete);
            }
        }
    }

    /**
     * Gets how markers can be on map
     * @param lengthOfArray
     * @returns amount of markers whitch can be on the map
     */
    private getMaximumMarkersOnMap(lengthOfArray: number): number {
        if (this.allowManyMarkers) {
            return (lengthOfArray > this._maximumConcurentRequestsToGoogleMap) ? this._maximumConcurentRequestsToGoogleMap : lengthOfArray;
        } else {
            return 1;
        }
    }

    /**
     * Gets the formatted addresses by coordinates from the _markers
     */
    private saveFormattedAddresses(): void {
        this._addresses = [];
        var maximumMarkersOnMap = this.getMaximumMarkersOnMap(this._markers.length);
        for (let i = 0; i < maximumMarkersOnMap; i++) {
            this._mapsAPILoader.load().then(() => {
                var location: LatLngLiteral = {
                    lat: this._markers[i].lat,
                    lng: this._markers[i].lng
                };
                var geocoder = new google.maps.Geocoder();
                geocoder.geocode({ 'location': location }, (results, status) => {
                    if (this.allowManyMarkers) {
                        var addressContainsInArray = this._addresses.find(a => a == results[0].formatted_address);
                        if (!addressContainsInArray) {
                            this._addresses.push(results[0].formatted_address);
                        }
                    }
                    else {
                        this._addresses[0] = results[0].formatted_address;
                    }
                });
            });
        }
    }

    /**
     * Sets all markers on map if allowed many markers
     * Sets first marker in _addresses
     */
    private setMarkersFromAddresses(): void {
        var maximumMarkersOnMap = this.getMaximumMarkersOnMap(this._addresses.length);
        for (let i = 0; i < maximumMarkersOnMap; i++) {
            this._mapsAPILoader.load().then(() => {
                var geocoder = new google.maps.Geocoder();
                geocoder.geocode({ 'address': this._addresses[i] }, (results, status) => {
                    console.log(results);
                    let newMarker = this.createNewMarker(results[0].geometry.location.lat(), results[0].geometry.location.lng(), results[0].formatted_address);
                    this._markers.push(newMarker);
                });
            });
        }
    }

    /**
     * Moves marker to place where was click if not allowed many markers
     * Add new marker in _markers if allowed many markers
     * @param $event
     */
    private mapClicked($event: any): void {
        if (this._markers.length < this._maximumConcurentRequestsToGoogleMap) {
            if (this.allowManyMarkers) {
                this._markers.push(this.createNewMarker($event.coords.lat, $event.coords.lng));
            } else {
                this.setMainPointer($event.coords.lat, $event.coords.lng);
                this._markers.push(this.createNewMarker(this.mainPointerLatitude, this.mainPointerLongitude));
            }
        }
    }

    /**
     * Creates new instance of marker
     * @param latitude
     * @param longitude
     * @returns new marker
     */
    private createNewMarker(latitude: number, longitude: number, name?: string): IMarker {
        return {
            name: name != null ? name : "Нова мітка",
            draggable: true,
            lat: latitude,
            lng: longitude
        };
    }

    /**
     * Changing coordinates when the marker was moved
     * @param $event
     */
    private markerDragEnd(marker: any, $event: any): void {
        var updatedMarker = this.createNewMarker(parseFloat(marker.lat), parseFloat(marker.lng));
        var newLatitude = $event.coords.lat;
        var newLongitude = $event.coords.lng;

        for (var i = 0; i < this._markers.length; i++) {
            if (updatedMarker.lat == this._markers[i].lat && updatedMarker.lng == this._markers[i].lng) {
                this._markers[i].lat = newLatitude;
                this._markers[i].lng = newLongitude;
            }
        }
    }

    /**
     * Removed marker from the map
     * @param marker
     */
    private removeMarker(marker: any): void {
        this._markers.splice(this._markers.findIndex(m => m.lat == parseFloat(marker.lat)), 1);
    }

    /**
     * Delete all markers from the map
     */
    private clearMapFromMarkers(): void {
        this._markers = [];
    }

    /**
     * Display all adresses on the map.
     * @param addresses: string[]
     */
    public setMarkers(addresses: string[]): void {
        this._addresses = addresses;
        this.setMarkersFromAddresses();
    }

    /**
     * Save all addresses by markers which setted on the map
     */
    public saveAllAddressesFromMarkers(): void {
        this.saveFormattedAddresses();
    }

    /**
     * Gets Array of formatted addresses
     * @returns array with addresses
     */
    public getAllAddresses(): string[] {
        return this._addresses;
    }
}


