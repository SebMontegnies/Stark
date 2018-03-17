$(function () {

	$('.toggle').each(function () {
		$(this).children('span').click(function () {
			$(this).parent().children('span').removeClass('selected');
			$(this).addClass('selected');
			$('input[name="' + $(this).parent().data('field') + '"]').val($(this).data('value'));
		});
	});

	$.ajax({
		type: "GET",
		url: "http://diseaseit.azurewebsites.net/api/active",
		data: { enable: false },
		success: function (activeResult) {
			console.log(activeResult);
		}
	});

});