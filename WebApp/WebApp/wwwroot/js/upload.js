$(function () {

	$('.toggle').each(function () {
		$(this).children('span').click(function () {
			$(this).parent().children('span').removeClass('selected');
			$(this).addClass('selected');
			$('input[name="' + $(this).parent().data('field') + '"]').val($(this).data('value'));
		});
	});

	$('#file').change(function () {
		$('#file-name').text($('#file')[0].files[0].name);
		if ($('#file')[0].files[0].name != '') {
			$('#file-loader').css({ 'display': 'inline-block' });

			$.ajax({
				type: "POST",
				url: "http://localhost:2921/api/FaceRecognition",
				data: { data: imageData },
				success: function (result) {
				}
			});
		}
	});

});