var video = document.querySelector("#human-video-stream");
navigator.getUserMedia = navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.mozGetUserMedia || navigator.msGetUserMedia || navigator.oGetUserMedia;

if (navigator.getUserMedia)
	navigator.getUserMedia({ video: true, audio: false }, handleVideo, videoError);

function startCaptureCountdown() {
	var timerDefault = 5;
	var timerCounter = 0;
	$('#human-capture-countdown').text(timerDefault);
	var downloadTimer = setInterval(function () {
		timerCounter++;
		if (timerCounter < timerDefault)
			$('#human-capture-countdown').text(timerDefault - timerCounter);
		else {
			clearInterval(downloadTimer);
			$('#human-capture-countdown').text('');
			snapshot();
		}
	}, 1000);
}

function handleVideo(stream) {
	video.src = window.URL.createObjectURL(stream);
	webcamStream = stream;
}

function videoError(e) {
	console.log('error video :');
	console.log(e);
}

function getSensors() {
	$.ajax({
		type: "GET",
		url: "http://diseaseit.azurewebsites.net/api/health/",
		success: function (healthResult) {
			var temp = parseFloat(healthResult.temperature).toFixed(2);
			$('#sensors-informations').append('<div class="temperature">' + temp + ' °C</div>');

			$('input[name="temperature"]').val(temp);

			$('#sensors-informations').append('<div class="heart-rate">' + healthResult.hearbeat + ' BPM</div>');
			$('input[name="hearbeat"]').val(healthResult.hearbeat);

			var bloodOx = parseFloat(healthResult.bloodoxygenationRate).toFixed(2);
			$('#sensors-informations').append('<div class="spo2">' + bloodOx + ' %</div>');
			$('input[name="bloodoxygenationRate"]').val(bloodOx);
		}
	});
}

function refreshSensors() {
	$.ajax({
		type: "GET",
		url: "http://diseaseit.azurewebsites.net/api/health/",
		success: function (healthResult) {
			var temp = parseFloat(healthResult.temperature).toFixed(2);
			$('#sensors-informations .temperature').text(temp + ' °C');
			$('input[name="temperature"]').val(temp);

			$('#sensors-informations .heart-rate').text(healthResult.hearbeat + ' BPM');
			$('input[name="hearbeat"]').val(healthResult.hearbeat);

			var bloodOx = parseFloat(healthResult.bloodoxygenationRate).toFixed(2);
			$('#sensors-informations .spo2').text(bloodOx + ' %');
			$('input[name="bloodoxygenationRate"]').val(bloodOx);

			if (healthResult.temperature == 0 && healthResult.hearbeat == 0 && healthResult.bloodoxygenationRate == 0)
				$('#sensors-error').show();
			else
				$('#sensors-error').hide();
		}
	});
}

function snapshot() {
	var canvas = document.getElementById("human-capture-canvas");
	var ctx = canvas.getContext('2d');
	ctx.drawImage(video, 0, 0, canvas.width, canvas.height);
	var imageData = canvas.toDataURL('image/jpeg');

	$.ajax({
		type: "POST",
		url: "http://diseaseit.azurewebsites.net/api/FaceRecognition",
		data: { data: imageData },
		beforeSend: function () {
			$('#human-video-stream-container').fadeOut(250);
			$('#human-body-light').addClass('scanning');
		},
		success: function (result) {
			if (result.gender == 0) {
				$('#human-body-man').show();
				$('#human-informations').append('<div class="gender man">Man</div>');
				$('input[name="gender"]').val('0');
			} else if (result.gender == 1) {
				$('#human-body-woman').show();
				$('#human-informations').append('<div class="gender woman">Woman</div>');
				$('input[name="gender"]').val('1');
			}

			$('#human-informations').append('<div class="age">' + result.age + ' Y</div>');
			$('input[name="age"]').val(result.age);

			$('#next-button').fadeIn();
		}
	});
}

$(function () {

	$.ajax({
		type: "POST",
		url: "http://diseaseit.azurewebsites.net/api/active",
		data: { enable: true },
		success: function (activeResult) {
			console.log(activeResult);
		}
	});

	getSensors();
	setInterval(function () {
		refreshSensors();
	}, 1000);

	$('#human-video-button').click(function () {
		$(this).fadeOut();
		$('#human-video-stream-container').fadeIn();
		startCaptureCountdown();
	});

});