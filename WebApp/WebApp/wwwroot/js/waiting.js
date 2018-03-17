function getRandomInt(min, max) {
	return Math.floor(Math.random() * (max - min + 1)) + min;
}

$(function () {

	for (i = 0; i < 30; i++) {
		setTimeout(function () {
			var newElement = $('#element-template').clone();
			var leftOffset = getRandomInt(150, $(window).width() - 150);
			var topOffset = getRandomInt(150, $(window).height() - 150);

			newElement.css({ top: topOffset, left: leftOffset });
			var oldHtml = newElement.html();
			var avatarType = getRandomInt(1, 2) == 1 ? '/images/avatar_girl.png' : '/images/avatar.png';
			var newHtml = oldHtml.replace('##3##', getRandomInt(15, 100)).replace('##4##', avatarType);
			newElement.html(newHtml).appendTo('#world-map');
			newElement.removeClass('hidden');
		}, i*150);
	}

});