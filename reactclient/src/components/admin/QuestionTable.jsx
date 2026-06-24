import { Link } from 'react-router-dom';

import { questionTypeLabel } from '../../utils/questionTypes';

import { TableSkeleton } from '../loading';

export default function QuestionTable({
  questions,

  selected,

  onToggle,

  onToggleAll,

  onDeleteOne,

  loading,

  domainTitleById = {},
}) {
  const allSelected = questions.length > 0 && selected.size === questions.length;

  if (loading) {
    return (
      <div className="overflow-x-auto loading-enter" role="status" aria-live="polite">
        <table className="w-full text-left text-sm">
          <thead className="bg-surface-container-low text-on-surface-variant text-xs uppercase tracking-wider">
            <tr>
              <th className="px-lg py-md w-10" />

              <th className="px-lg py-md font-semibold">Title</th>

              <th className="px-lg py-md font-semibold">Domain</th>

              <th className="px-lg py-md font-semibold">Type</th>

              <th className="px-lg py-md font-semibold">Options</th>

              <th className="px-lg py-md font-semibold text-right">Actions</th>
            </tr>
          </thead>

          <tbody className="divide-y divide-outline-variant">
            <TableSkeleton rows={6} columns={6} />
          </tbody>
        </table>

        <span className="sr-only">Loading questions…</span>
      </div>
    );
  }

  if (questions.length === 0) {
    return (
      <p className="text-on-surface-variant text-sm p-lg">
        No questions yet. Import or add questions from the Question Pool.
      </p>
    );
  }

  return (
    <div className="overflow-x-auto content-reveal">
      <table className="w-full text-left text-sm">
        <thead className="bg-surface-container-low text-on-surface-variant text-xs uppercase tracking-wider">
          <tr>
            <th className="px-lg py-md w-10">
              <input
                type="checkbox"
                checked={allSelected}
                onChange={onToggleAll}
                className="rounded border-outline-variant"
              />
            </th>

            <th className="px-lg py-md font-semibold">Title</th>

            <th className="px-lg py-md font-semibold">Domain</th>

            <th className="px-lg py-md font-semibold">Type</th>

            <th className="px-lg py-md font-semibold">Options</th>

            <th className="px-lg py-md font-semibold text-right">Actions</th>
          </tr>
        </thead>

        <tbody className="divide-y divide-outline-variant">
          {questions.map((q) => (
            <tr key={q.id} className="hover:bg-surface-container-low/50">
              <td className="px-lg py-md">
                <input
                  type="checkbox"
                  checked={selected.has(q.id)}
                  onChange={() => onToggle(q.id)}
                  className="rounded border-outline-variant"
                />
              </td>

              <td className="px-lg py-md max-w-md">
                <Link
                  to={`/admin/questions/${q.id}`}
                  className="font-medium text-primary hover:text-secondary-container line-clamp-2"
                >
                  {q.title || 'Untitled'}
                </Link>
              </td>

              <td className="px-lg py-md text-on-surface-variant">
                <div>{q.domainTitle || domainTitleById[q.domainId] || '—'}</div>

                {q.examTitle && (
                  <div className="text-xs text-on-surface-variant/80">{q.examTitle}</div>
                )}
              </td>

              <td className="px-lg py-md text-on-surface-variant">
                {questionTypeLabel(q.questionType)}
              </td>

              <td className="px-lg py-md text-on-surface-variant">{q.answerOptionCount ?? 0}</td>

              <td className="px-lg py-md text-right">
                <Link
                  to={`/admin/questions/${q.id}`}
                  className="text-xs font-bold text-secondary-container hover:underline mr-md"
                >
                  Edit
                </Link>

                <button
                  type="button"
                  onClick={() => onDeleteOne(q)}
                  className="text-xs font-bold text-red-600 hover:underline"
                >
                  Delete
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
