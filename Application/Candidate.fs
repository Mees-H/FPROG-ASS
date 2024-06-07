module Rommulbad.Application.Candidate

open Rommulbad.Model.Model
open Rommulbad.Model.Common

type ICandidateDataAccess =
    abstract AddCandidate: string -> string -> string -> Option<Candidate>
    abstract GetCandidates: unit -> List<Candidate>
    abstract GetCandidate: string -> Option<Candidate>
    abstract GetEligibleMinutesPerCandidate: List<Candidate> -> List<int> -> List<Candidate * int>
    abstract PutCandidateDiploma: string -> string -> Option<Candidate>

let addCandidate (store: ICandidateDataAccess) (name: string) (guardianId: string) (diploma: string): Option<Candidate> =
    store.AddCandidate (name) (guardianId) (diploma)

let getCandidates (store: ICandidateDataAccess): List<Candidate> =
    store.GetCandidates ()

let getCandidate (store: ICandidateDataAccess) (name: string): Option<Candidate> =
    store.GetCandidate (name)

let getEligibleMinutesPerCandidate (store: ICandidateDataAccess) (candidates: List<Candidate>) (minutes: List<int>): List<Candidate * int> =
    store.GetEligibleMinutesPerCandidate (candidates) (minutes)

let putCandidateDiploma (store: ICandidateDataAccess) (name: string) (diploma: string): Option<Candidate> =
    store.PutCandidateDiploma (name) (diploma)