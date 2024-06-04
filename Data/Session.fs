module Rommulbad.Data.Session

open Rommulbad.Application.Session
open Rommulbad.Model.Model
open Rommulbad.Data.Database
open Rommulbad.Data.Store

open System

let sessionPersistance = 
    { new ISessionDataAccess with
        member this.AddSession (name: string) (deep: bool) (date: DateTime) (minutes: int) = 
            let result = InMemoryDatabase.insert (name, date) (name, deep, date, minutes) store.sessions
            match result with
            | Error _ -> None
            | Ok _-> Some { Session.Deep = deep
                            Date = date
                            Minutes = minutes }


        member this.GetSessions (name: string) = 
            InMemoryDatabase.all store.sessions
            |> Seq.filter (fun (n, _, _, _) -> n = name)
            |> Seq.map (fun (_, deep, date, minutes) ->
                { Session.Deep = deep
                  Date = date
                  Minutes = minutes })
            |> Seq.toList

        member this.GetTotalMinutes (name: string) = 
            InMemoryDatabase.all store.sessions
            |> Seq.filter (fun (n, _, _, _) -> n = name)
            |> Seq.map (fun (_, _, _, minutes) -> minutes)
            |> Seq.sum

        member this.GetEligibleSessions (name: string) (shallowOk: bool) (minMinutes: int) = 
            InMemoryDatabase.all store.sessions
            |> Seq.filter (fun (n, _, _, _) -> n = name)
            |> Seq.map (fun (_, deep, date, minutes) ->
                { Session.Deep = deep
                  Date = date
                  Minutes = minutes })
            |> Seq.toList

        member this.GetTotalEligibleMinutes (name: string) (shallowOk: bool) (minMinutes: int) = 
            InMemoryDatabase.all store.sessions
            |> Seq.filter (fun (n, _, _, _) -> n = name)
            |> Seq.map (fun (_, _, _, minutes) -> minutes)
            |> Seq.sum
    }

