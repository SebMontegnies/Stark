function getRandomInt(min, max) {
	return Math.floor(Math.random() * (max - min + 1)) + min;
}

$(function () {

	for (i = 0; i < 20; i++) {
		setTimeout(function () {
			var newElement = $('#element-template').clone();
			var leftOffset = getRandomInt(150, $(window).width() - 150);
			var topOffset = getRandomInt(150, $(window).height() - 150);

			newElement.removeClass('hidden');
			newElement.css({ top: topOffset, left: leftOffset });
			var oldHtml = newElement.html();
			var newHtml = oldHtml.replace('##1##', 'test').replace('##2##', 'Patient n°' + i).replace('##3##', getRandomInt(15, 100));
			newElement.html(newHtml).appendTo('#world-map');
		}, i*850);
	}

});