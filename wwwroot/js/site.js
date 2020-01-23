// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function validate(evt) {
    var theEvent = evt || window.event;

    // Handle paste
    if (theEvent.type === 'paste') {
        key = event.clipboardData.getData('text/plain');
    } else {
        // Handle key press
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
    }
    var regex = /[0-9]|\./;
    if (!regex.test(key)) {

        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}

const savePointsScore = () => {
	try {
		let msg = '';
		if ($('#scoreDate').val() == "") {
			msg = 'Preencha o campo de data';
		}
		if ($('#scorePoints').val() == "") {
			msg = 'Preencha o campo de pontos';
		}

		if (msg == '') {
			let data = {};
			data.date = $('#scoreDate').val();
			data.points = $('#scorePoints').val();

			$('#btn-score').html('<div class="spinner-border text-white" role="status"><span class="sr-only"></span></div>');
			$('#btn-score').prop("disabled", true);
			$.ajax({
				url: $('#form-score').attr('action'),
				method: "POST",
				data: data,
				beforeSend: function () {
				},
				success: function (response) {
					let alert = "danger";
					let status = "Falha";
					if (response.status == 'suc') {
						alert = "success";
						status = "Sucesso";
					} else {
						alert = "warning";
						status = "Atenção";
					}
					let div = "response-box";
					responseBox(status, div, alert, response.msg);
				},
				error: function (xhr, ajaxOptions, thrownError) {
					console.log(xhr.status);
					console.log(xhr.responseText);
				},
				complete: function () {
					$('#btn-score').html('Salvar');
					$('#btn-score').prop("disabled", false);
				}
			});
		} else {
			toastr.warning(msg, 'Atenção', { positionClass: 'toast-top-right', timeOut: 2000 });
		}
	} catch (err) {
		let status = "Falha";
		let alert = "danger";
		let div = "response-box";
		responseBox(status, div, alert, err.message);
	}
}

const deletePoints = () => {
	try {
		let data = {};

		$('#btn-scoredel').html('<div class="spinner-border text-white" role="status"><span class="sr-only"></span></div>');
		$('#btn-scoredel').prop("disabled", true);
		$.ajax({
			url: window.location.origin + '/Home/DeletePoints',
			method: "POST",
			data: data,
			beforeSend: function () {
			},
			success: function (response) {
				let alert = "danger";
				let status = "Falha";
				if (response.status == 'suc') {
					alert = "success";
					status = "Sucesso";
				}
				else {
					alert = "warning";
					status = "Atenção";
				}
				let div = "response-box";
				responseBox(status,div, alert, response.msg);
			},
			error: function (xhr, ajaxOptions, thrownError) {
				console.log(xhr.status);
				console.log(xhr.responseText);
			},
			complete: function () {
				$('#btn-scoredel').html('Excluir Pontos');
				$('#btn-scoredel').prop("disabled", false);
			}
		});
	} catch (err) {
		let status = "Falha";
		let alert = "danger";
		let div = "response-box";
		responseBox(status,div, alert, err.message);
	}
}

const responseBox = (status,div,alert,msg) => {
	let html = `<div class="alert alert-${alert} alert-dismissible fade show text-break" role="alert"><strong>${status}!</strong>${msg}<button type = "button" class="close" data-dismiss="alert" aria-label="Close" ><span aria-hidden="true">&times;</span></button ></div >`;
	$('#' + div).html(html);
	setTimeout(() => { $('#' + div).html(''); }, 10000);
}