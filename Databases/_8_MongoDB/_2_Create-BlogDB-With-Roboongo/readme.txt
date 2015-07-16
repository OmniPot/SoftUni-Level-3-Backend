Using Robomongo you cannot simply insert Date(), because it is not a valid json
so I did it using the command line inside Robomongo like that

db.blog.insert({
    "title" : "Dummy post one created.... ouuuu YEAAAHH",
    "content" : "I am Vancho the good looking guy the one that never looks bad!",
    "createdAt" : Date(),
    "category" : "Nonsense",
    "tags" : [ 
        "non", 
        "sense", 
        "cooool"
    ],
    "author" : {
        "displayName" : "Ivan Georgiev Stanimirov",
        "tweeterAcc" : "vankataaaaaaGS",
        "linkedInAcc" : "www.linkedin.com/profile/view?id=12461346146"
    }
})

db.blog.insert({
    "title" : "Dummy post on the way to be created by MEEEEEE",
    "content" : "Coll post bro, keep up the good work!!",
    "createdAt" : Date(),
    "category" : "Human",
    "tags" : [ 
        "H", 
        "U", 
        "M", 
        "A", 
        "N"
    ],
    "author" : {
        "displayName" : "Steliqn Sti Sta Ste",
        "tweeterAcc" : "stelianStoqnov",
        "linkedInAcc" : "www.linkedin.com/profile/view?id=12151616"
    }
}