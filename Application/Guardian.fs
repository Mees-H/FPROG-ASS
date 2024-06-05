module Rommulbad.Application.Guardian

open Rommulbad.Model.Model

type IGuardianDataAccess =
    abstract AddGuardian: string -> string -> Option<Guardian>

let addGuardian (store: IGuardianDataAccess) (id: string) (name: string): Option<Guardian> =
    store.AddGuardian id name 