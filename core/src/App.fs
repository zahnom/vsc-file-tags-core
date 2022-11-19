namespace vsc_file_tags_fsharp

module App =

    open System.Collections.Generic

    let tags = new List<Tag>()

    let tagFile path name =
        tags.Add({ Name = name; File = path })
        tags

    let getAllTags = tags

    let selectTags name tags = 
        tags |> Seq.filter (fun x -> x.Name = name)
