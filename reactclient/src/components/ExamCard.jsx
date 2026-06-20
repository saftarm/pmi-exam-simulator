import { Link } from 'react-router-dom';
import Icon from './Icon';
import LoadingButton from './loading/LoadingButton';

const EXAM_IMAGES = [
    'https://lh3.googleusercontent.com/aida-public/AB6AXuDUVgGY-vKI_8p4s3pQx9r4UwKXE8lMkqOtTDYTGhAoKKQL3V5e8g4n1QEXhbdlvO-7UB1xDnApSFSHwWAYQO87q8CKmhYv_1jW07Wqjb9J0QVanvcG7mXOy8hjvuEW9F6uN-oYDFlsA2RyPtxGgAjBRg_irfrHJvAmJqjeRR83adZrUrTqXb3c0gqwl9BkZZ93GwFRMdYdZXDhwiMvNDy4sSIMaZmF2dRHpLDKu48Imm_fOs0KPK5zvR-M9Bl529udaNpTwvSWSZw',
    'https://lh3.googleusercontent.com/aida-public/AB6AXuBM9iqioZvO7V91NCzGS0iy6EAE8iao6fXUnp2YsPlB5BJZKsc-f-ffq14eCtg0ctNQ1kIFp8qcuyiBK_Ns9rEioM30zv81AU9y3j5sU5bJDc6PSt61Na7SfM49MqK6MaTCwPohFP9PaqMb1a9zGo2SnzK4QJdxi0Y26Ns3EnQfKmLJ7ZMSyTnyiCfqT0NELcee-NaQYbjTHyYxo4MP0wf7LKPryWcIgz3KEArCn8DiZVHmYOb3RPCFgD9gzWFwWn3PtPtXZMVNf2o',
    'https://lh3.googleusercontent.com/aida-public/AB6AXuDpzjAgpgj7TAY_u9NOrdL-efzKchc3KDU2nlho4Tl-kiSiqYkCtO0J-HmUNKFHFXdlyFOUAK6SqAKawxWF8Cjm9puC4p2RwmG2AKdzWZMFDr40RlUqUVGZDJSuXbSjXmkMD8y6JqW_LFk4m9lJ6T66v713XaxVeKJDlIPvvkGM4h9YMLYrlmaD-ppDTjdkYSX7vtqthAyQM3Q0VAbm-ChDwNvGu7hHJIfSsScMnA1p220XS0qP3H5a-1zCw9fIGF60rSEqUgjc780',
];

const BADGES = ['MOST POPULAR', 'FOUNDATIONAL', 'AGILE SPECIALIST'];

const META_ICONS = [
    ['quiz', 'schedule', 'trending_up', 'language'],
    ['quiz', 'schedule', 'bar_chart', 'book'],
    ['quiz', 'schedule', 'sync', 'verified_user'],
];

const META_LABELS = [
    ['Questions', 'Mins', 'Expert Level', 'Global Standards'],
    ['Questions', 'Mins', 'Associate Level', 'PMBOK Guide'],
    ['Questions', 'Mins', 'Agile/Scrum', 'Elite Spec'],
];

export default function ExamCard({ exam, index = 0, onStart, starting }) {
    const imageIndex = index % EXAM_IMAGES.length;
    const badge = BADGES[imageIndex] ?? 'CERTIFICATION';
    const isPopular = imageIndex === 0;

    const metaIcons = META_ICONS[imageIndex];
    const metaLabels = META_LABELS[imageIndex];

    const metaValues = [
        `${exam.numberOfQuestions ?? 0} Questions`,
        `${exam.durationInMinutes ?? 0} Mins`,
        metaLabels[2],
        metaLabels[3],
    ];

    return (
        <div className="bg-white rounded-xl border border-outline-variant overflow-hidden flex flex-col card-hover transition-all duration-300">
            <div className="h-48 relative overflow-hidden bg-primary-container">
                <img
                    className="w-full h-full object-cover opacity-80"
                    src={EXAM_IMAGES[imageIndex]}
                    alt={exam.title ?? 'Exam'}
                />
                <div className="absolute top-md left-md">
                    <span className={`text-label-sm font-label-sm px-md py-xs rounded-full ${
                        isPopular
                            ? 'bg-secondary-container text-on-secondary'
                            : 'bg-surface-container-high text-on-surface-variant'
                    }`}
                    >
                        {badge}
                    </span>
                </div>
            </div>
            <div className="p-lg flex flex-col flex-grow">
                <h2 className="font-headline-lg text-headline-lg text-primary mb-xs">{exam.title}</h2>
                <p className="font-label-lg text-label-lg text-on-surface-variant mb-lg">
                    {exam.context || 'PMI Certification Exam'}
                </p>
                <div className="grid grid-cols-2 gap-md mb-xl">
                    {metaValues.map((value, i) => (
                        <div key={value} className="flex items-center gap-xs text-on-surface-variant">
                            <Icon name={metaIcons[i]} style={{ fontSize: 18 }} />
                            <span className="font-label-sm text-label-sm">{value}</span>
                        </div>
                    ))}
                </div>
                <div className="mt-auto pt-lg border-t border-outline-variant space-y-sm">
                    <Link
                        to={`/exams/${exam.id}`}
                        className="w-full block text-center py-sm rounded-lg border border-outline-variant font-label-lg text-label-lg text-primary hover:bg-surface-container-low transition-colors"
                    >
                        View details
                    </Link>
                    <LoadingButton
                        onClick={() => onStart(exam.id)}
                        loading={starting}
                        loadingText="Launching…"
                        className="w-full bg-secondary-container hover:brightness-110 active:scale-[0.98] transition-all text-on-secondary font-label-lg text-label-lg py-md rounded-lg disabled:opacity-60"
                    >
                        Launch Simulator
                        {!starting && <Icon name="play_arrow" style={{ fontSize: 20 }} />}
                    </LoadingButton>
                </div>
            </div>
        </div>
    );
}
