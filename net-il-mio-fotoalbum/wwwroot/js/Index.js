loadPhoto();

function loadPhoto(searchKey) {
    axios.get('/Api/PhotoApi/GetPhoto', {
        params: {
            search: searchKey
        }
    })
        .then((res) => {        //se la richiesta va a buon fine
            console.log('risposta ok', res);
            if (res.data.length == 0) {     //non ci sono post da mostrare => nascondo la tabella
                document.getElementById('photo-table').classList.add('d-none');
                document.getElementById('no-photo').classList.remove('d-none');
            } else {                        //ci sono post da mostrare => visualizzo la tabella
                document.getElementById('photo-table').classList.remove('d-none');
                document.getElementById('no-photo').classList.add('d-none');

                //svuoto la tabella
                document.getElementById('photo').innerHTML = '';
                res.data.forEach(Photo => {
                    console.log('Photo', Photo);
                    document.getElementById('photo').innerHTML +=
                        `
                        <tr>
                            <td>
                                <a href="/Client/Details?id=${Photo.id}">${Photo.id}</a>
                            </td>
                            <td class="image"><img style="width:60px; height:45px;" src=${Photo.image}></td>
                            <td class="title">${Photo.title}</td>
                            <td class="description">${Photo.description}</td> 
                        </tr>
                        `;
                })
            }
        })
        .catch((res) => {       //se la richiesta non è andata a buon fine
            console.error('errore', res);
        });

}
