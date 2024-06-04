module Rommulbad.Application.Session

open Rommulbad.Model.Model
open System

type ISessionDataAccess =
    abstract AddSession: string -> bool -> DateTime -> int -> Option<Session>
    abstract GetSessions: string -> List<Session>
    abstract GetTotalMinutes: string -> int
    abstract GetEligibleSessions: string -> bool -> int -> List<Session>
    abstract GetTotalEligibleMinutes: string -> bool -> int -> int

let addSession (store: ISessionDataAccess) (name: string) (deep: bool) (date: DateTime) (minutes: int): Option<Session> =
    store.AddSession name deep date minutes

let getSessions (store: ISessionDataAccess) (name: string): List<Session> =
    store.GetSessions name

let getTotalMinutes (store: ISessionDataAccess) (name: string): int =
    store.GetTotalMinutes name

let getEligibleSessions (store: ISessionDataAccess) (name: string) (shallowOk: bool) (minMinutes: int): List<Session> =
    store.GetEligibleSessions name shallowOk minMinutes

let getTotalEligibleMinutes (store: ISessionDataAccess) (name: string) (shallowOk: bool) (minMinutes: int): int =
    store.GetTotalEligibleMinutes name shallowOk minMinutes