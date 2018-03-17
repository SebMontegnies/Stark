$(function () {

	$('.toggle').each(function () {
		$(this).children('span').click(function () {
			$(this).parent().children('span').removeClass('selected');
			$(this).addClass('selected');
			$('input[name="' + $(this).parent().data('field') + '"]').val($(this).data('value'));
		});
	});

});