﻿namespace vsc_file_tags_fsharp

module Test =

    #if FABLE_COMPILER
    open Fable.Mocha
    #else
    open Expecto
    #endif

    let uri path =
        new System.Uri(path)
    
    let ``Tag file tests`` =
        testSequenced <| testList "A file was tagged" [

            test "with one tag" {
                App.tags.Clear()
        
                let file = "my/file.txt"
                let name = "a tag"
                    
                App.tagFile file name |> ignore

                Expect.equal App.getAllTags.Count 1 "then only one tag must exist"
                Expect.equal App.getAllTags.[0] { Name = name; File = file } "then the added tag must exists"
            }
    
            test "with four unique tags" {
                App.tags.Clear()

                let file1 = "my/file1.txt"
                let file2 = "my/file2.txt"
                let name1 = "a tag"
                let name2 = "another tag" 
                let name3 = "yet another tag"

                App.tagFile file1 name1 |> ignore
                App.tagFile file1 name2 |> ignore
                App.tagFile file2 name2 |> ignore
                App.tagFile file2 name3 |> ignore

                Expect.equal App.getAllTags.Count 4 "then four tags must exist"
                Expect.isTrue (App.getAllTags |> Seq.contains { Name = name1; File = file1 }) "then the correct first tag exists"
                Expect.isTrue (App.getAllTags |> Seq.contains { Name = name2; File = file1 }) "then the correct second tag exists"
                Expect.isTrue (App.getAllTags |> Seq.contains { Name = name2; File = file2 }) "then the correct third tag exists"
                Expect.isTrue (App.getAllTags |> Seq.contains { Name = name3; File = file2 }) "then the correct fourth tag exists"
            }
        ]

    let ``Filter tests`` =
        testSequenced <| testList "Tags were filtered" [

            test "by their name" {
                App.tags.Clear()

                App.tagFile "my/file1.txt" "x" |> ignore
                App.tagFile "my/file2.txt" "-" |> ignore
                App.tagFile "my/file3.txt" "x" |> ignore
                App.tagFile "my/file4.txt" "x" |> ignore
                App.tagFile "my/file5.txt" "-" |> ignore

                Expect.equal (App.getAllTags |> App.selectTags "x" |> Seq.length) 3 "so three tags should remain"
                Expect.isTrue (
                    App.getAllTags
                    |> App.selectTags "x"
                    |> Seq.map (fun x -> x.File)
                    |> Seq.contains "my/file1.txt") "so file 1 should be in the filtered list"
            }
        ]

    [<EntryPoint>]
    let main args =
    #if FABLE_COMPILER
        Mocha.runTests ``Tag file tests``
        Mocha.runTests ``Filter tests``
    #else
        runTestsWithArgs defaultConfig args ``Tag file tests``
        runTestsWithArgs defaultConfig args ``Filter tests``
    #endif

