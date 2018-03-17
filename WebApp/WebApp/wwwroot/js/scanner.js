var video = document.querySelector("#human-video-stream");
navigator.getUserMedia = navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.mozGetUserMedia || navigator.msGetUserMedia || navigator.oGetUserMedia;

if (navigator.getUserMedia) {
	navigator.getUserMedia({ video: true, audio: false }, handleVideo, videoError);
	$('#human-video-stream-container').fadeIn();
}

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
	startCaptureCountdown();
}

function videoError(e) {
	console.log('error video :');
	console.log(e);
}

function snapshot() {
	var canvas = document.getElementById("human-capture-canvas");
	var ctx = canvas.getContext('2d');
	ctx.drawImage(video, 0, 0, canvas.width, canvas.height);
	var imageData = canvas.toDataURL('image/jpeg');

	$.ajax({
		type: "POST",
		url: "http://localhost:8000/api/FaceRecognition",
		data: { data: imageData },
		success: function (result) {

			$('#human-video-stream-container').fadeOut(250);
			$('#human-body-light').addClass('scanning');

			if (result.gender == 0) {
				$('#human-body-man').show();
				setTimeout(function () {
					$('#human-informations').append('<div class="gender man">Man</div>');
				}, 1500);
				$('input[name="gender"]').val('0');
			} else if (result.gender == 1) {
				$('#human-body-woman').show();
				setTimeout(function () {
					$('#human-informations').append('<div class="gender woman">Woman</div>');
				}, 1500);
				$('input[name="gender"]').val('1');
			}

			setTimeout(function () {
				$('#human-informations').append('<div class="age">' + result.age + ' Y</div>');
			}, 3000);
			$('input[name="age"]').val(result.age);

			$.ajax({
				type: "GET",
				url: "http://diseaseit.azurewebsites.net/api/health/",
				success: function (healthResult) {
					setTimeout(function () {
						$('#sensors-informations').append('<div class="temperature">' + healthResult.temperature + ' °C</div>');
					}, 1500);
					$('input[name="temperature"]').val(healthResult.temperature);

					setTimeout(function () {
						$('#sensors-informations').append('<div class="heart-rate">' + healthResult.hearbeat + ' BPM</div>');
					}, 3000);
					$('input[name="hearbeat"]').val(healthResult.hearbeat);

					setTimeout(function () {
						$('#sensors-informations').append('<div class="spo2">' + healthResult.bloodoxygenationRate + ' %</div>');
					}, 4500);
					$('input[name="bloodoxygenationRate"]').val(healthResult.bloodoxygenationRate);
				}
			});

			setTimeout(function () {
				$('#human-body-light').removeClass('scanning');
				$('#next-button').fadeIn();
			}, 9500);
			
		}
	});
}

$(function () {

	$.ajax({
		type: "GET",
		url: "http://diseaseit.azurewebsites.net/api/active",
		data: {},
		success: function (activeResult) {
			console.log(activeResult);
		}
	});

});