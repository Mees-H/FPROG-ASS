module Rommulbad.Application.Candidate

open Rommulbad.Model.Model
open Rommulbad.Model.Common

type ICandidateDataAccess =
    abstract GetCandidates: unit -> List<Candidate>
    abstract GetCandidate: string -> Option<Candidate>

let getCandidates (store: ICandidateDataAccess): List<Candidate> =
    store.GetCandidates ()

let getCandidate (store: ICandidateDataAccess) (name: string): Option<Candidate> =
    store.GetCandidate (name)