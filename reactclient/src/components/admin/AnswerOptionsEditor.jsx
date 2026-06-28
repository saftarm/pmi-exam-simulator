import {
  updateAnswerOption,
  addAnswerOption,
  removeAnswerOption,
} from '../../utils/questionFormState';

export default function AnswerOptionsEditor({ options, onChange }) {
  const updateOption = (index, field, value) => {
    onChange(updateAnswerOption(options, index, field, value));
  };

  return (
    <div>
      <div className="flex justify-between items-center mb-sm">
        <label className="block text-sm font-bold">Answer options</label>
        <button
          type="button"
          onClick={() => onChange(addAnswerOption(options))}
          className="text-sm font-bold text-secondary-container hover:underline"
        >
          + Add option
        </button>
      </div>
      <div className="space-y-sm">
        {options.map((opt, index) => (
          <div
            key={opt.id || `new-${index}`}
            className="flex flex-col sm:flex-row gap-sm border border-outline-variant rounded-lg p-md"
          >
            <input
              value={opt.text}
              onChange={(e) => updateOption(index, 'text', e.target.value)}
              placeholder="Answer text"
              className="flex-1 border border-outline-variant rounded-lg px-md py-sm text-sm"
            />
            <label className="flex items-center gap-sm text-sm shrink-0">
              <input
                type="checkbox"
                checked={opt.isCorrect}
                onChange={(e) => updateOption(index, 'isCorrect', e.target.checked)}
              />
              Correct
            </label>
            <button
              type="button"
              onClick={() => onChange(removeAnswerOption(options, index))}
              className="text-xs font-bold text-red-600"
            >
              Remove
            </button>
          </div>
        ))}
      </div>
    </div>
  );
}
