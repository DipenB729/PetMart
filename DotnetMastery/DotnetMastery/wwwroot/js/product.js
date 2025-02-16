$(document).ready(function ()){
    loadDataTable();
}

function loadDataTable() {
    dataTable = $('#tblTable').DataTable({
        "ajax": { url: '/admin/product/getall' },
        "column":[
            { data: 'title',"width":"15%" },
            { data: 'position', "width": "15%" },
            { data: 'salary', "width": "15%" },
            { data: 'office', "width": "15%" }
        ]
    })
}