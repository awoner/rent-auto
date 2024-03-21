db.createUser(
    {
        user: "devroot",
        pwd: "devroot",
        roles: [
            {
                role: "readWrite",
                db: "rentauto"
            }
        ]
    }
);

db.createCollection('users');
