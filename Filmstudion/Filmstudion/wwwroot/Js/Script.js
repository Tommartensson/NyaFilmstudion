let main = document.getElementById("main")
let Password = document.getElementById("Pass")
let Login = document.getElementById("CheckIf")
let user = document.getElementById("User")
let username = document.getElementById("Username")
let pass = document.getElementById("password")
let city = document.getElementById("city")
let button = document.getElementById("button")
let FilmstudioName = document.getElementById("FilmstudioName")
let allFilms = document.getElementById("AllFilms")

/*Films*/
allFilms.innerHTML = showFilms();

function showFilms (Token, userId)
{
fetch("../api/Films")
.then(Response => Response.json())
.then(films => {
  
    if(Token)
    {
    main.insertAdjacentHTML("afterbegin", "<h1 id=myMovies> Mina filmer </div>")
    let movies = document.getElementById("myMovies")
    movies.addEventListener("click", function(){
        main.innerHTML = ""
        showMyMovies(Token, userId);
    })
    let logout = document.createElement("button")
            logout.innerHTML = "Logga ut"
            logout.addEventListener("click", function(){
                location.reload();
            })
}
    else
    {
        console.log(Token)
    }
    for(i = 0; i < films.length; i++)
    {
        main.insertAdjacentHTML("afterbegin", "<div id='"+films[i].filmId +"'>" + films[i].name  + "</br>" + films[i].director + "</br>")
        if(Token && userId)
        {
            let button = document.createElement("button")
            button.setAttribute("id", films[i].filmId)
            button.innerHTML = "Låna";
            main.insertAdjacentElement("afterbegin", button)
            button.addEventListener("click", function(event){
                event.preventDefault()
                console.log(event.target.id)
                let FilmId = parseInt(event.target.id)
                fetch("../api/Films/rent?id=" + FilmId + "&studioId=" + userId, 
                {method: "POST",
                headers: {"Content-type": "application/json; charset=UTF-8", "Authorization": "Bearer " + Token}})
            })
         
        }
        console.log(films)
       
        
    
    }
})
}




/*Authentication */




Login.addEventListener("click", function(){
    const user = {
        Username: username.value,
        password: Password.value
    }
    fetch("../api/Users/Authenticate", {method: "POST",
    body: JSON.stringify(user),
    headers: {"Content-type": "application/json; charset=UTF-8"}})
    .then(Response => Response.json())
    .then( isAuth => {
            let id = isAuth.userId
            let Token = isAuth.token
            main.innerHTML = showFilms(Token, id)
    })
    .catch(alert("Du kunde inte logga in"))
})

button.addEventListener("click", function() {
    let data = {username: username.value, 
                Password: pass.value,
                FilmStudioCity: city.value,
                FilmStudioName: FilmstudioName.value
                }
                console.log(data)
    fetch("../api/filmstudio/register", {method: "POST",
    body: JSON.stringify(data),
    headers: {"Content-type": "application/json; charset=UTF-8"}})
    .then(Response => Response.json())
    .then(json => console.log(json))
    .catch(err => console.log(err))
    
    
})

/*My Movies*/ 
function showMyMovies(Token, userId)
{
    fetch("../api/mystudio/rentals",{ method: "GET",
    headers: {"Content-type": "application/json; charset=UTF-8", "Authorization": "Bearer " + Token}})
    .then(Response => Response.json())
    .then(Films => {
        for(i=0; i < Films.length; i++)
        {
            fetch("../api/Films/" + Films[i].filmId)
            .then(Response => Response.json())
            .then(myfilms => {
            main.insertAdjacentHTML("afterbegin", "<div id='"+myfilms.filmId +"'>" + myfilms.name  + "</br>" + myfilms.director + "</br>")  
            let button = document.createElement("button")
            button.setAttribute("id", myfilms.filmId)
            button.innerHTML = "Låna";
            main.insertAdjacentElement("afterbegin", button)
            button.addEventListener("click", function(event){
                event.preventDefault()
                console.log(event.target.id)
                let FilmId = parseInt(event.target.id)
                fetch("../api/Films/return?id=" + FilmId + "&studioId=" + userId, 
                {method: "POST",
                headers: {"Content-type": "application/json; charset=UTF-8", "Authorization": "Bearer " + Token}})
            })})
        }
        })
    main.insertAdjacentHTML("afterbegin", "<h1 id=GoBack> Gå tillbaka </div>")
    let movies = document.getElementById("GoBack")
    movies.addEventListener("click", function(){
        main.innerHTML = ""
        showFilms(Token, userId);
    })
}