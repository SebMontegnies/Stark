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
					console.log(healthResult);
					gg = healthResult;
					setTimeout(function () {
						var temp = parseFloat(healthResult.temperature).toFixed(2);
						$('#sensors-informations').append('<div class="temperature">' + temp + ' °C</div>');

						$('input[name="temperature"]').val(temp);
					}, 1500);
			

					setTimeout(function () {
						$('#sensors-informations').append('<div class="heart-rate">' + healthResult.hearbeat + ' BPM</div>');
						$('input[name="hearbeat"]').val(healthResult.hearbeat);
					}, 3000);
			

					setTimeout(function () {
						var bloodOx = parseFloat(healthResult.bloodoxygenationRate).toFixed(2);
						$('#sensors-informations').append('<div class="spo2">' + bloodOx + ' %</div>');
						$('input[name="bloodoxygenationRate"]').val(bloodOx);
					}, 4500);
				
				
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
		type: "POST",
		url: "http://diseaseit.azurewebsites.net/api/active",
		data: {enable:true},
		success: function (activeResult) {
			console.log(activeResult);
		}
	});

});

function closeCapteur()
{
	$.ajax({
		type: "POST",
		url: "http://diseaseit.azurewebsites.net/api/active",
		data: { enable: false },
		success: function (activeResult) {
			console.log(activeResult);
		}
	});
	return true;
}