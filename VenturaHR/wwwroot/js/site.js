// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function initializeMask() {
    $('.cpf').mask('000.000.000-00', { placeholder: "___.___.___-__" });
    $('.rg').mask('00.000.000-0', { reverse: true, placeholder: "__.___.___-_" });
    $('.cnh').mask('000000000-00', { reverse: true, placeholder: "_________-__" });
    $('.cnpj').mask('00.000.000/0000-00', { reverse: true, placeholder: "__.___.___/____-__" });
    $('.money').mask('#.##0,00', { reverse: true });
    $('.date').mask('00/00/0000', { clearIfNotMatch: true, placeholder: "__/__/____" });
    $('.date-no-placeholder').mask('00/00/0000', { clearIfNotMatch: true });
    $('.month-datepicker').mask('00/0000', { clearIfNotMatch: true, placeholder: "__/____" });
    $('.year').mask('9999', { clearIfNotMatch: true });
    $('.time').mask('99:99:99');
    $('.time-no-seconds').mask('99:99', {
        clearIfNotMatch: true,
        onComplete: function (time, event, input) {
            $('.error-time-no-seconds').remove();

            if (isNullOrWhiteSpace(time) || !moment(time, 'HH:mm')._isValid) {
                console.log(input);
                $(input).val('');
                $(input).after('<small class="text-danger error-time-no-seconds">Horário inválido.</small>');
            }
        }
    });

    $('.percent').mask('##0,00%', { reverse: true, placeholder: '_,__%' });

    $('.cep').mask('00000-000', { placeholder: "_____-___" });
    $('.number').mask('#');

    /*$('.date, .date-no-placeholder').datepicker({ format: 'dd/mm/yyyy', language: 'pt-BR', autoclose: true });*/
    //$('.month-datepicker').datepicker({ format: 'mm/yyyy', minViewMode: 1, language: 'pt-BR', autoclose: true });
    //$('.year').datepicker({ format: 'yyyy', minViewMode: 2, language: 'pt-BR', autoclose: true });

    $('.phone').mask('(00) 0000-0000', { placeholder: "(__) ____-____" });
    $('.mobile').mask('(00) 00000-0000', { placeholder: "(__) _____-____" });

    var CpfCnpjMaskBehavior = function (val) {
        return val.replace(/\D/g, '').length > 11 || val.replace(/\D/g, '').length == 0 ? '00.000.000/0000-00' : '000.000.000-009';
    },
        ccOptions = {
            onKeyPress: function (val, e, field, options) {
                field.mask(CpfCnpjMaskBehavior.apply({}, arguments), options);
            }
        };

    $('.cpfcnpj').mask(CpfCnpjMaskBehavior, ccOptions);
}

$(document).ready(initializeMask);

function initializeCopyToClipboard() {
    $('[data-copy-to-clipboard]').off('click');

    $('[data-copy-to-clipboard]').click(function () {
        var input = $('<input>', { value: $(this).data('text') });

        $('body').append(input);

        $(input).select();

        document.execCommand("copy");

        bootbox.alert('O link foi copiado.');
        $(input).remove();
    });
}

$(document).ready(initializeCopyToClipboard);

try {
    $.extend(true, $.fn.dataTable.defaults, {
        "iDisplayLength": 10,
        'language': {
            "sEmptyTable": "Nenhum registro encontrado",
            "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
            "sInfoFiltered": "(Filtrados de _MAX_ registros)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "_MENU_ resultados por página",
            "sLoadingRecords": "Carregando...",
            "sProcessing": "Processando...",
            "sZeroRecords": "Nenhum registro encontrado",
            "sSearch": "Pesquisar",
            "oPaginate": {
                "sNext": "Próximo",
                "sPrevious": "Anterior",
                "sFirst": "Primeiro",
                "sLast": "Último"
            },
            "oAria": {
                "sSortAscending": ": Ordenar colunas de forma ascendente",
                "sSortDescending": ": Ordenar colunas de forma descendente"
            }
        }
    });
} catch { }