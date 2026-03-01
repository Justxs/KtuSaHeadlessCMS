document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('[data-quill-editor]').forEach(function (container) {
        let editorEl = container.querySelector('[data-quill-target]');
        let hiddenInput = container.querySelector('[data-quill-input]');
        let initialContent = hiddenInput.value;

        let quill = new Quill(editorEl, {
            modules: {
                toolbar: [
                    [{ 'header': [1, false] }],
                    ['bold', 'underline', 'italic'],
                    ['link', 'video'],
                    [{ 'list': 'ordered' }]
                ],
                clipboard: {
                    matchVisual: false,
                    matchers: [
                        [Node.ELEMENT_NODE, function (_node, delta) {
                            return delta.compose(
                                new Delta().retain(delta.length(), {
                                    bold: false,
                                    italic: false,
                                    underline: false,
                                    link: false
                                })
                            );
                        }]
                    ]
                }
            },
            theme: 'snow'
        });

        quill.root.innerHTML = initialContent;

        quill.on('text-change', function () {
            hiddenInput.value = quill.root.innerHTML;
        });
    });
});
