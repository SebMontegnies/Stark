$(function () {

	$('.toggle').each(function () {
		$(this).children('span').click(function () {
			$(this).parent().children('span').removeClass('selected');
			$(this).addClass('selected');
			$('input[name="' + $(this).parent().data('field') + '"]').val($(this).data('value'));
			if ($(this).hasClass('green')) {
				$('.toggle').each(function () {
					$(this).children('span.green').removeClass('selected');
					$(this).children('span.red').addClass('selected');
				});
				$(this).parent().children('span.red').removeClass('selected');
				$(this).parent().children('span.green').addClass('selected');
			}
		});
	});

});