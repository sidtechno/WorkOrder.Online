document.addEventListener("DOMContentLoaded",
    function () {
        var e = document.querySelectorAll("[data-trigger]");
        for (i = 0; i < e.length; ++i) {
            var a = e[i];
            new Choices(a, { placeholderValue: "", searchPlaceholderValue: "" })
        }
        new Choices("#choices-single-no-search",
            {
                searchEnabled: !1, removeItemButton: !0,
                choices: [{ value: "One", label: "Label One" }, { value: "Two", label: "Label Two", disabled: !0 }, { value: "Three", label: "Label Three" }]
            }).setChoices([{ value: "Four", label: "Label Four", disabled: !0 }, { value: "Five", label: "Label Five" },
                { value: "Six", label: "Label Six", selected: !0 }], "value", "label", !1),
            new Choices("#choices-single-no-sorting", { shouldSort: !1 }), new Choices("#choices-multiple-remove-button", { removeItemButton: !0 }), new Choices(document.getElementById("choices-multiple-groups")), new Choices(document.getElementById("choices-text-remove-button"), { delimiter: ",", editItems: !0, maxItemCount: 5, removeItemButton: !0 }), new Choices("#choices-text-unique-values", { paste: !1, duplicateItemsAllowed: !1, editItems: !0 }), new Choices("#choices-text-disabled", { addItems: !1, removeItems: !1 }).disable()
    }); 