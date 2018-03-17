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
		if (timerCounter <= timerDefault)
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

function stopVideo() {
	webcamStream.stop();
}

function snapshot() {
	var canvas = document.getElementById("human-capture-canvas");
	var ctx = canvas.getContext('2d');
	ctx.drawImage(video, 0, 0, canvas.width, canvas.height);
	stopVideo();
}

$(function () {

	

});