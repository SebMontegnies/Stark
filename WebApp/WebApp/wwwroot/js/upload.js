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
		file = $('#file')[0].files[0];
		if (file !== null) {
			$('#file-loader').css({ 'display': 'inline-block' });
	
			var reader = new FileReader();
			reader.onload = function () {
				$.ajax({
					url: '/attachmentURL',
					type: 'POST',
					data: reader.result
				});
			}
			console.log(reader.readAsBinaryString(file));
			//$.ajax({
			//	type: "POST",
			//	url: "http://localhost:2921/api/FaceRecognition",
			//	data: { data: data },
			//	success: function (result) {

			var documentName = '<span>' + file.name + '</span>';
			var symptomResult = 'Souffle au coeur';
			var symptom = '<span>' + symptomResult + '</span>'; //result.symptom
			$('#files-list').append('<div class="file">' + documentName + symptom + '</div>');

			//	}
			//});
		}
	});

});