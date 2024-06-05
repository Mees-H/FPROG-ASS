module Rommulbad.Service.Routes

open Giraffe

let routes: HttpHandler =
    choose
        [ Candidate.candidateRoutes
          Session.sessionRoutes
          Guardian.guardianRoutes ]